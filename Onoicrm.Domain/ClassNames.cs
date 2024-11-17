namespace Onoicrm.Domain;

public static class ClassNames
{
    public const int
        UserProfile = 1,
        Clinic = 2,
        ClinicUserProfile = 3,
        Group = 4,
        GroupUserProfile = 5,
        UserProfileGroup = 6,
        Tooth = 7,
        Symptom = 8,
        ToothBooking = 9,
        PatientSymptom = 10,
        ToothState = 11,
        Price = 12,
        Armchair = 13,
        Booking = 14,
        File = 15,
        BookingFile = 16,
        Payment = 19,
        Service = 20,
        BookingService = 21,
        CancellationReason = 22,
        TreatmentPlan = 23,
        ServiceGroup = 24,
        ServiceGroupLink = 25,
        BookingServiceGroup = 21,
        ImplementedService = 22,
        Channel = 23,
        DoctorServiceSalary=24,
        BookingGroup=25,
        Patient=26,
        Doctor=27;
}

public static class StateNames
{
    public const int
        Created = 1,
        Activated = 2,
        Deleted = 3,
        Accepted = 4,
        InProgress=5,
        Done=6,
        Reserved=7,
        Canceled=8;
}