namespace NSI.Domain.DTOs.Master
{
    // Opsi: Kamu bisa mengganti 'class' menjadi 'record' jika ingin immutability penuh 
    // dan performa komparasi yang lebih baik secara bawaan (C# 9+).
    public class MSCourseDto
    {
        public string? PlantCode { get; init; }

        // Gunakan 'required' untuk properti wajib, bukan '= null!'
        public string? ClusterCode { get; init; }

        public string? CourseCode { get; init; }
        public string? Title { get; init; }
        public string? DocNo { get; init; }
        public string? Frequency { get; init; }
        public string? ShiftCls { get; init; }
        public string? StatusShift { get; init; }
        public DateTime RegisterDate { get; init; }
        public string? RegisterUser { get; init; }
        public DateTime LastUpdate { get; init; }
        public string? LastUser { get; init; }
    }

    public class MSCourseFilterClusterDto
    {
        public required string ClusterCode { get; init; }
        public required string ClusterDesc { get; init; }
    }

    public class MSCourseFilterPlantDto
    {
        public string? PlantCode { get; init; }
        public string? PlantDesc { get; init; }
    }

    public class MSCourseFilterFrequencyDto
    {
        public required string FrequencyCode { get; init; }
        public required string FrequencyDesc { get; init; }
    }
}