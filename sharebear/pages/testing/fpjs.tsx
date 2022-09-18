import { useVisitorData } from "@fingerprintjs/fingerprintjs-pro-react"
import { useState } from 'react'

export default function Fpjs() {
    const { isLoading, error, data, getData } = useVisitorData({
        extendedResult: true
    }, { immediate: true })

    return (
        <div>
            <pre>{error ? error.message : JSON.stringify(data, null, 2)}</pre>
        </div>
    )
}