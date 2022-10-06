import { AxiosError, AxiosResponse } from "axios";
import { FileWithPath } from "react-dropzone";
import { RequestError } from "../types/axiosTypes";
import { ContainerHubsDto, GetStorageStatisticsResponse } from "../types/containerTypes";
import axios from "./axios";

enum RestMethod {
    GET,
    POST
}
export async function GetServiceFreeSpace() {
    const { data } = await axios.get<GetStorageStatisticsResponse>(`/api/file/storage`)

    return data
}

export async function CreateContainer(files: FileWithPath[], visitorId: string) {
    let formData = new FormData();
    for (const file in files) {
        formData.append("FormFiles", files[file]);
    }

    return await RequestWrapper<ContainerHubsDto, FormData>(RestMethod.POST, `/api/file/container/create`, formData, {
        'Content-Type': 'multipart/form-data',
        'VisitorIdHeader': visitorId
    })
}

export async function GetContainerShort(shortRequestCode: string) {
    return await RequestWrapper<ContainerHubsDto>(RestMethod.GET, `/api/file/container?shortRequestCode=${shortRequestCode}`)

}



export async function GetContainerLong(longRequestCode: string) {
    return await RequestWrapper<ContainerHubsDto>(RestMethod.GET, `/api/file/container?longRequestCode=${longRequestCode}`)
}



export type AxiosRequestHeaders = Record<string, string | number | boolean>;

async function RequestWrapper<T>(method: RestMethod.GET, url: string, headers?: AxiosRequestHeaders): Promise<[T | null, RequestError | null]>
async function RequestWrapper<T, R>(method: RestMethod.POST, url: string, body: R, headers?: AxiosRequestHeaders): Promise<[T | null, RequestError | null]>
async function RequestWrapper<T, R>(method: RestMethod, url: string, body?: R, headers?: AxiosRequestHeaders): Promise<[T | null, RequestError | null]> {
    try {

        const data = method == RestMethod.GET ?
            await axios.get<T>(url) :
            await axios.post<T>(url, body, {
                headers: headers
            })

        return [data.data, null]
    }
    catch (ex: any) {
        if (ex instanceof AxiosError) {
            return [null, {
                message: ex.response?.data.message ?? ex.message,
                statusCode: ex.response?.status ?? null
            }]
        }

        return [null, ex]
    }
}