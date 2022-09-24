import { Container, Box, Stack, Typography, Button } from '@mui/material'
import styles from '../../../styles/JoinBucket.module.css'
import HoneyPotIcon from '../icons/HoneyPotIcon'
import { useEffect, useRef, useState } from "react"


export default function JoinBucket() {
    const itemsRef = useRef<Array<HTMLDivElement | null>>([])
    const codeElements = [
        0, 1, 2, 3, 4, 5
    ]
    const [code, setCode] = useState<string>()

    useEffect(() => {
        itemsRef.current = itemsRef.current.slice(0, codeElements.length);
    }, [codeElements]);

    return (
        <div className={styles.joinBucketContainer}>
            <div className={styles.upperRightShape} />
            <div className={styles.lowerRightShape} />
            <div className={styles.nestShape} />

            <Container sx={{
                zIndex: 500,
                height: "100%"
            }} maxWidth="lg">

                <Box sx={{
                    height: "100%",
                    display: "flex",
                    alignItems: { xs: "flex-start", md: "center" },
                    justifyContent: { xs: "center", md: "flex-start" }
                }}>
                    <Stack
                        direction="column"
                        justifyContent="center">
                        <Typography
                            mb={5}
                            variant="h3"
                            sx={{
                                zIndex: 500,

                                fontFamily: 'Aref Ruqaa Ink',
                                fontSize: {
                                    // xs: "1.5rem",
                                    // md: "2.125rem",
                                },
                                color: "#502720",
                            }}
                        >
                            <Box>
                                Join bucket
                            </Box>

                        </Typography>

                        <Typography
                            variant="h5"
                            mb={1}
                            sx={{
                                zIndex: 500,

                                fontFamily: 'Aref Ruqaa Ink',
                                fontSize: {
                                    // xs: "1.5rem",
                                    // md: "2.125rem",
                                },
                                color: "#502720",
                            }}
                        >
                            <Box>
                                Enter your code here:
                            </Box>
                        </Typography>

                        <Box sx={{
                            zIndex: 50
                        }} mb={5}>
                            {[...Array(6)].map((item, i) => (
                                <input onKeyDown={(e: any) => {
                                    if (e.key == "e" || e.key == "E") {
                                        e.preventDefault()
                                        return
                                    }

                                    if (e.code == "Backspace" || e.key == "Backspace") {
                                        e.preventDefault()
                                        e.target.value = ""
                                        itemsRef.current[i - 1]?.focus()
                                    }
                                }} onChange={(e) => {
                                    if (e.target.value)
                                        itemsRef.current[i + 1]?.focus()
                                    else
                                        e.target.value = ""
                                }} ref={el => itemsRef.current[i] = el} key={i} type="number" maxLength={1} className={styles.customInputBox} />
                            ))}
                        </Box>
                    </Stack>
                </Box>
            </Container>
        </div>
    )
}