import React from 'react';
import { CreateContainer } from '../../src/services/fileService';
import { useVisitorData } from '@fingerprintjs/fingerprintjs-pro-react';

const TestingFileUpload = () => {
    const { isLoading, error, data, getData } = useVisitorData({
        extendedResult: true
    }, { immediate: true })


    const handleFileSelect = async (event: any) => {
        const fileEvent = event.target as HTMLInputElement

        if (!fileEvent.files || !fileEvent.files?.length) {
            // error no file uploaded
            return;
        }

        if (!data?.visitorId)
            return

        const res = await CreateContainer(fileEvent.files, data.visitorId)
        console.log(res)
    }

    return (
        <form>
            <input type="file" multiple onChange={handleFileSelect} />
        </form>
    )
};

export default TestingFileUpload;