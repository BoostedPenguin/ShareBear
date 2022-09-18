namespace ShareBear.Dtos
{
    public partial class ContainerFilesDto
    {
        public string ContainerFileName { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        // Extension
        public string FileType { get; set; } = string.Empty;
        
        // In bytes
        public long FileSize { get; set; }

        // The signed download link
        public string SignedItemUrl { get; set; } = string.Empty;
    }
}
