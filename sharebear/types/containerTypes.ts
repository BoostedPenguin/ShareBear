export interface ContainerHubsDto {
    shortCodeString: string;
    fullCodeString: string;
    containerName: string;
    createdByVisitorId: string;
    expiresAt: string;
    createdAt: string;
    containerFiles: ContainerFilesDto[];
}

export interface ContainerFilesDto {
    containerFileName: string;
    fileName: string;
    fileType: string;
    fileSize: number;
    signedItemUrl: string;
}

export interface GetStorageStatisticsResponse {
    maxStorageBytes: number;
    usedStorageBytes: number;
    hasFreeSpace: boolean;
}