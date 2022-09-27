import { ContainerHubsDto } from "../../../types/containerTypes";
import styles from '../../../styles/Bucket.module.css'
import { Container, Box, Stack, Typography, Button, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper } from "@mui/material";
import { scroller } from "react-scroll";
import HoneyPotIcon from "../icons/HoneyPotIcon";
import HiveIcon from "../icons/HiveIcon";
import { formatBytes } from "../../helpers/sizeCalculator";
import { useEffect, useState } from "react";

export default function BucketView(data: { container: ContainerHubsDto }) {
    const [time, setTime] = useState<string>()

    useEffect(() => {
        CountDownTimer(data.container.expiresAt, "")
    }, [data.container.expiresAt])

    // CountDownTimer('02/19/2012 10:1 AM', 'countdown');
    // CountDownTimer('02/20/2012 10:1 AM', 'newcountdown');

    function CountDownTimer(dt: string, id: string) {
        var end = new Date(dt);

        var _second = 1000;
        var _minute = _second * 60;
        var _hour = _minute * 60;
        var _day = _hour * 24;
        let timer: NodeJS.Timer;

        function showRemaining() {
            var now = new Date();
            var distance = end.getTime() - now.getTime();
            if (distance < 0) {

                clearInterval(timer);
                //document.getElementById(id).innerHTML = 'EXPIRED!';

                return;
            }
            var days = Math.floor(distance / _day);
            var hours = Math.floor((distance % _day) / _hour);
            var minutes = Math.floor((distance % _hour) / _minute);
            var seconds = Math.floor((distance % _minute) / _second);

            setTime(`${days} days, ${hours} hours, ${minutes} minutes, ${seconds} seconds`)
        }

        timer = setInterval(showRemaining, 1000);
    }



    return (
        <div className={styles.homeContainer}>
            <div className={styles.lowerRightShape} />
            <div className={styles.upperRightShape} />
            <div className={styles.bearWithTable} />
            <Container sx={{
                zIndex: 500,
                height: "100%"
            }} maxWidth="xl">

                <Box sx={{
                    height: "100%",
                    display: "flex",
                    pt: 2,
                    justifyContent: { xs: "center", md: "flex-start" }
                }}>
                    <Stack
                        direction="column"
                        alignItems="center"
                        spacing={4}>
                        <Typography
                            variant="h4"
                            sx={{
                                fontFamily: 'Aref Ruqaa Ink',
                                maxWidth: 500,
                                zIndex: 1,
                                color: "#502720",
                                textAlign: "center",
                            }}
                        >
                            <Box sx={{
                                display: "flex",
                                fontSize: {
                                    xs: "1.5rem",
                                    md: "2.125rem",
                                },
                            }} alignItems={"center"}>

                                <b>You joined a bucket</b>
                                <HiveIcon height={60} width={60} />
                            </Box>

                            <Box sx={{
                                fontSize: {
                                    xs: "1.5rem",
                                    md: "1.5rem",
                                },
                            }} mt={2}>
                                Bucket will expire in:
                                <Box>
                                    {time}
                                </Box>
                            </Box>

                        </Typography>

                        {/* List here */}
                        <TableContainer sx={{
                            backgroundColor: "#E2B590",
                            // width: "50vw",
                            minWidth: "50vw",
                            zIndex: 50
                        }} component={Paper} >
                            <Table stickyHeader aria-label="sticky table">
                                <TableHead sx={{
                                    "& th": {
                                        // color: "rgba(96, 96, 96)",
                                        backgroundColor: "#CEA27F"
                                    }
                                }}>
                                    <TableRow>
                                        <TableCell
                                        >
                                            File name
                                        </TableCell>
                                        <TableCell align="right"
                                        >
                                            Type
                                        </TableCell>
                                        <TableCell align="right"
                                        >
                                            Size
                                        </TableCell>
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    {data.container.containerFiles.map(e =>
                                        <TableRow key={e.fileName} hover role="checkbox" tabIndex={-1} >
                                            <TableCell>
                                                {e.fileName}
                                            </TableCell>
                                            <TableCell align="right">
                                                {e.fileType}
                                            </TableCell>
                                            <TableCell align="right">
                                                {formatBytes(e.fileSize)}
                                            </TableCell>
                                        </TableRow>
                                    )}

                                </TableBody>
                            </Table>
                        </TableContainer>
                    </Stack>
                </Box>
            </Container>
        </div>
    )
}