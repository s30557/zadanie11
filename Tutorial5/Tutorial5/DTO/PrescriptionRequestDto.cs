namespace Tutorial5.DTO;

public class PrescriptionRequestDto {
    public string PatientFirstName { get; set; }
    public string PatientLastName { get; set; }
    public DateTime PatientBirthdate { get; set; }

    public int DoctorId { get; set; }

    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }

    public List<PrescriptionMedicamentDto> Medicaments { get; set; }
}