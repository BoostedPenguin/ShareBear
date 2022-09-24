import Box from '@mui/material/Box';
import Stack from '@mui/material/Stack';
import Container from '@mui/material/Container';
import styles from '../../../styles/CreateBucket.module.css'
import Typography from '@mui/material/Typography';
import React from 'react';
import HoneyPotIcon from '../icons/HoneyPotIcon';
import { Alert } from '@mui/material';
import useAvailableStorage from '../hooks/useAvailableStorage';
import useCreateBucket from '../hooks/useCreateBucket';


export default function CreateBucket() {
    const availableStorage = useAvailableStorage()

    const {
        dragZoneError,
        dragZoneText,
        dropZoneStyle,
        getRootProps,
        isDragAccept,
    } = useCreateBucket()

    return (
        <div className={styles.createBucketContainer} >
            <div className={styles.lowerRightHoneycomb} />
            <div className={styles.upperLeftHoneyDrop} />

            <Container sx={{
                height: "100%",

            }} maxWidth="lg">
                <Stack spacing={5} sx={{
                    height: "100%",
                    justifyContent: "center",
                    alignContent: "center",
                }}>
                    <Box sx={{
                        zIndex: 50,
                    }}>
                        <Typography
                            variant="h3"
                            sx={{
                                fontFamily: 'Aref Ruqaa Ink',
                                fontSize: {
                                    // xs: "1.5rem",
                                    // md: "2.125rem",
                                },
                                color: "#502720",
                                textAlign: "center",
                            }}
                        >
                            <Box>
                                Create bucket
                            </Box>

                        </Typography>
                    </Box>

                    {/* On error */}
                    {dragZoneError && <Alert variant="filled" severity="error" sx={{
                        zIndex: 50,
                    }}>
                        {dragZoneError}
                    </Alert>}

                    {/* On not enough storage storage */}
                    {availableStorage.data && !availableStorage.data.hasFreeSpace && <Alert variant="filled" severity="warning" sx={{
                        zIndex: 50,
                    }}>
                        Currently there isn&apos;t any available storage for new buckets. Check back later.
                    </Alert>}

                    {/* On available storage information not available */}
                    {availableStorage.isError && <Alert variant="filled" severity="warning" sx={{
                        zIndex: 50,
                    }}>
                        Error contacting the server. Check back later.
                    </Alert>}

                    <Box sx={{
                        zIndex: 50,

                        display: "flex",
                        height: "50%",
                        alignItems: "center",
                        justifyContent: "center"
                    }}>

                        <div  {...getRootProps({
                            style: {
                                ...dropZoneStyle,
                                flexDirection: "column"
                            }
                        })}>
                            <Box sx={{
                                fontSize: {
                                    md: isDragAccept ? "1.6rem" : "1.2rem"
                                }
                            }} textAlign={"center"}>
                                {dragZoneText}
                            </Box>
                            <HoneyPotIcon height={100} width={100} />

                        </div>
                    </Box>
                    <Box sx={{
                        zIndex: 50,
                    }}>
                        * Max container size 50mb
                    </Box>
                </Stack>


            </Container>
        </div>
    )
}