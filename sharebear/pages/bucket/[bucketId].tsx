import { AxiosError } from "axios"
import { GetServerSideProps } from "next"
import { useRouter } from "next/router"
import { GetContainerLong } from "../../lib/fileService"
import BucketView from "../../src/components/Bucket/BuckerView"
import useBucket from "../../src/components/hooks/useBucket"
import { RequestError } from "../../types/axiosTypes"
import { ContainerHubsDto } from "../../types/containerTypes"
import { mockContainerHub } from "../../src/helpers/mockedTypes"

interface BucketProps {
    data: ContainerHubsDto | null,
    error: RequestError | null,
}

export default function Bucket({ data, error }: BucketProps) {

    if (error) {
        return (
            <>
                <BucketView container={mockContainerHub} />
            </>
        )
    }
    return (
        <>
            Found your bucket
            <br />
            {JSON.stringify(data)}
        </>
    )
}

export const getServerSideProps: GetServerSideProps = async (context) => {
    const bucketId = context.query.bucketId
    if (!bucketId || bucketId instanceof Array)
        return {
            props: {
                error: true,
                message: "Wrong page"
            }
        }
    const [data, error] = await GetContainerLong(bucketId)

    if (error)
        return {
            props: {
                error
            }
        }

    return {
        props: {
            data
        }
    }
}