import axios from "axios"
import { ContainerHubsDto } from "../types/containerTypes"

const baseUrl = process.env.NEXT_PUBLIC_BACKEND_BASE_URL

export async function CreateContainer(files: FileList, visitorId: string) {

    var formData = new FormData();
    for(const file in files) {
        formData.append("FormFiles", files[file]);
    }

    return await axios.post<ContainerHubsDto>(`${baseUrl}/api/file/container/create`, formData, {
        headers: {
            'Content-Type': 'multipart/form-data',
            'VisitorIdHeader' : visitorId
        }
    })
}

export async function GetContainer(shortRequestCode: string) {
    return await axios.get<ContainerHubsDto>(`${baseUrl}/api/file/container?shortRequestCode=${shortRequestCode}`)
}