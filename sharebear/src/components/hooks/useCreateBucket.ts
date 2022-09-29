import { useVisitorData } from "@fingerprintjs/fingerprintjs-pro-react"
import { useQuery } from "@tanstack/react-query"
import { AxiosError } from "axios"
import { useCallback, useEffect, useMemo, useState } from "react"
import { FileRejection, FileWithPath, useDropzone } from "react-dropzone"
import { CreateContainer, GetServiceFreeSpace } from "../../../lib/fileService"
import { formatBytes } from "../../helpers/sizeCalculator"
import CreateBucket from "../Home/CreateBucket"

export default function useCreateBucket() {
    const maxFileSize: number = 50000000
    const [dragZoneText, setDragZoneText] = useState<string>()
    const [dragZoneError, setDragZoneError] = useState<string | undefined>()
    const { getData } = useVisitorData()


    const onDrop = useCallback(async (acceptedFiles: FileWithPath[], rejectedFiles: FileRejection[]) => {

        if (rejectedFiles.length > maxFileSize) {
            setDragZoneError(rejectedFiles.flatMap(e => e.errors.flatMap(y => y.message)).find(e => e.length > 0))
            return
        }

        const total = acceptedFiles.reduce((partial, current) => partial + current.size, 0)
        if (total > maxFileSize) {
            setDragZoneError(`The maximum size of each bucket can be only 50MBs. The size of your selected files is ${formatBytes(total)}`)
            return
        }
        // Do something with the files
        setDragZoneError(undefined);

        const visitorRequest = await getData()
        if (!visitorRequest || !visitorRequest.visitorId) {
            setDragZoneError("Error verifying user. Check back later.")
            return
        }

        const data = await CreateContainer(acceptedFiles, visitorRequest.visitorId).catch(ex => {
            setDragZoneError(ex.response?.data?.message || ex.message)
            return
        })
        console.log(data)
    }, [getData])

    const {
        getRootProps,
        getInputProps,
        isFocused,
        isDragAccept,
        isDragReject
    } = useDropzone({
        onDrop,

        // 50 mb
        maxSize: maxFileSize
    });



    const baseStyle = {
        flex: 1,
        display: 'flex',
        alignItems: 'center',
        justifyContent: "center",
        borderWidth: 2,
        borderRadius: 2,
        borderColor: '#502F57',
        borderStyle: 'dashed',
        backgroundColor: 'rgba(255, 255, 255, 0.43)',
        color: '#502F57',
        height: "100%",
        outline: 'none',
        transition: 'border .24s ease-in-out',
    };

    const focusedStyle = {
        borderColor: '#2196f3'
    };

    const acceptStyle = {
        borderWidth: 5,
        backgroundColor: 'rgba(142, 80, 69, 0.4)',
    };

    const rejectStyle = {
        borderColor: '#ff1744'
    };

    const dropZoneStyle = useMemo(() => ({
        ...baseStyle,
        ...(isFocused ? focusedStyle : {}),
        ...(isDragAccept ? acceptStyle : {}),
        ...(isDragReject ? rejectStyle : {})
    }), [
        isFocused,
        isDragAccept,
        isDragReject,

        focusedStyle,
        acceptStyle,
        rejectStyle
    ]);





    useEffect(() => {
        setDragZoneText(isDragAccept ? "Drop your files here" : "Drag 'n' drop some files here, or click to select files")
    }, [isDragAccept])


    return {
        getInputProps,
        dragZoneError,
        getRootProps,
        dropZoneStyle,
        isDragAccept,
        dragZoneText
    }
}