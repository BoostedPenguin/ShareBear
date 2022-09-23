import { FileWithPath } from "react-dropzone";
import { ContainerHubsDto, GetStorageStatisticsResponse } from "../types/containerTypes";
import axios from "./axios";


export async function GetServiceFreeSpace() {
    const { data } = await axios.get<GetStorageStatisticsResponse>(`/api/file/storage`)

    return data
}

export async function CreateContainer(files: FileWithPath[], visitorId: string) {
    let formData = new FormData();
    for (const file in files) {
        formData.append("FormFiles", files[file]);
    }

    return await axios.post<ContainerHubsDto>(`/api/file/container/create`, formData, {
        headers: {
            'Content-Type': 'multipart/form-data',
            'VisitorIdHeader': visitorId
        }
    })
}

export async function GetContainer(shortRequestCode: string) {
    return await axios.get<ContainerHubsDto>(`/api/file/container?shortRequestCode=${shortRequestCode}`)
}