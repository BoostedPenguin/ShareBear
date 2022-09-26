import { useRouter } from "next/router"
import useBucket from "../../src/components/hooks/useBucket"

export default function Bucket() {
    const router = useRouter()
    const bucket = useBucket()
    const { bucketId } = router.query
    return (
        <>
            {bucketId}
        </>
    )
}