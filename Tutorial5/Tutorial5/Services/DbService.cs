using Microsoft.EntityFrameworkCore;
using Tutorial5.Data;
using Tutorial5.DTO;
using Tutorial5.Models;

namespace Tutorial5.Services;

public class DbService : IDbService {
    private readonly DatabaseContext _context;

    public DbService(DatabaseContext context) {
        _context = context;
    }

    public async Task AddPrescriptionAsync(PrescriptionRequestDto dto) {
        if (dto.Medicaments.Count > 10)
            throw new ArgumentException("Maximum 10 medicaments per prescription");

        if (dto.DueDate < dto.Date)
            throw new ArgumentException("DueDate cannot be earlier than Date");

        var doctor = await _context.Doctors.FindAsync(dto.DoctorId);
        if (doctor == null)
            throw new ArgumentException("Doctor not found");

        var patient = await _context.Patients
                          .FirstOrDefaultAsync(p => p.FirstName == dto.PatientFirstName && p.LastName == dto.PatientLastName && p.Birthdate == dto.PatientBirthdate)
                      ?? new Patient {
                          FirstName = dto.PatientFirstName,
                          LastName = dto.PatientLastName,
                          Birthdate = dto.PatientBirthdate
                      };

        var prescription = new Prescription {
            Date = dto.Date,
            DueDate = dto.DueDate,
            Patient = patient,
            Doctor = doctor,
            PrescriptionMedicaments = new List<PrescriptionMedicament>()
        };

        foreach (var med in dto.Medicaments) {
            var medicament = await _context.Medicaments.FindAsync(med.IdMedicament);
            if (medicament == null)
                throw new ArgumentException($"Medicament {med.IdMedicament} not found");

            prescription.PrescriptionMedicaments.Add(new PrescriptionMedicament {
                Medicament = medicament,
                Dose = med.Dose,
                Description = med.Description
            });
        }

        _context.Prescriptions.Add(prescription);
        await _context.SaveChangesAsync();
    }
}
