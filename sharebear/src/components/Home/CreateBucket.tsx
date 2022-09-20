import Button from '@mui/material/Button'
import Box from '@mui/material/Box';
import Stack from '@mui/material/Stack';
import Container from '@mui/material/Container';
import styles from '../../../styles/CreateBucket.module.css'
import HoneyDrop from '../icons/honeyDrop.svg'
import Typography from '@mui/material/Typography';
import { useDropzone, FileWithPath } from 'react-dropzone';
import React, { useMemo, useCallback, useState, useEffect } from 'react';
import HoneyPotIcon from '../icons/HoneyPotIcon';


export default function CreateBucket() {

    const [dragZoneText, setDragZoneText] = useState<string>()
    const onDrop = useCallback((acceptedFiles: FileWithPath[]) => {
        console.log(acceptedFiles)
        // Do something with the files
    }, [])

    const {
        getRootProps,
        getInputProps,
        isFocused,
        isDragAccept,
        isDragReject
    } = useDropzone({
        onDrop
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
        borderColor: '#00e676'
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
        isDragReject
    ]);



    useEffect(() => {
        setDragZoneText(isDragAccept ? "Drop your files here" : "Drag 'n' drop some files here, or click to select files")
    }, [isDragAccept])

    return (
        <div className={styles.createBucketContainer} >
            <div className={styles.lowerRightHoneycomb} />
            <div className={styles.upperLeftHoneyDrop} />

            <Container sx={{
                height: "100%"
            }} maxWidth="xl">
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
                            {/* <input {...getInputProps()} /> */}
                            <Box color={isDragAccept ? "#fff" : "#000"} sx={{
                                fontSize: {
                                    md: "1.2rem"
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