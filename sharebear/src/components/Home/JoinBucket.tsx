import { Container, Box, Stack, Typography, Button, IconButton } from '@mui/material'
import styles from '../../../styles/JoinBucket.module.css'
import HoneyPotIcon from '../icons/HoneyPotIcon'
import { useEffect, useRef, useState } from "react"
import { SendRounded } from '@mui/icons-material';


export default function JoinBucket() {
    const itemsRef = useRef<Array<HTMLInputElement | null>>([])
    const [code, setCode] = useState<string>()


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
                                <input
                                    onInput={(e: any) => e.target.value = e.target.value.slice(0, 1)}
                                    onKeyDown={(e: any) => {
                                        if (e.key == "e" || e.key == "E") {
                                            e.preventDefault()
                                            return
                                        }

                                        if (e.code == "Backspace" || e.key == "Backspace") {
                                            e.preventDefault()
                                            if (e.target.value == "") {
                                                const previousItem = itemsRef.current[i - 1]
                                                if (previousItem)
                                                    previousItem.value = ""

                                                itemsRef.current[i - 1]?.focus()
                                                return
                                            }
                                            e.target.value = ""
                                            itemsRef.current[i - 1]?.focus()
                                        }

                                        if (e.target.value)
                                            itemsRef.current[i + 1]?.focus()
                                        else
                                            e.target.value = ""

                                    }} ref={el => itemsRef.current[i] = el} key={i} type="number" maxLength={1} className={styles.customInputBox} />
                            ))}
                            <IconButton aria-label="Join bucket" style={{
                                transform: "scale(1.8)"
                            }} sx={{
                                marginLeft: 2,
                                color: "#44254A",
                            }}>
                                <SendRounded />
                            </IconButton>
                        </Box>
                    </Stack>
                </Box>
            </Container>
        </div>
    )
}