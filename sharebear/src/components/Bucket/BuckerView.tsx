import { ContainerHubsDto } from "../../../types/containerTypes";
import styles from '../../../styles/Bucket.module.css'
import { Container, Box, Stack, Typography, Button, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, IconButton, Modal, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, TextField, Snackbar, Alert } from "@mui/material";
import { scroller } from "react-scroll";
import HoneyPotIcon from "../icons/HoneyPotIcon";
import HiveIcon from "../icons/HiveIcon";
import { formatBytes } from "../../helpers/sizeCalculator";
import { useEffect, useState } from "react";
import ShareIcon from '@mui/icons-material/Share';
import Grid from '@mui/material/Unstable_Grid2';
import { useRouter } from "next/router";
import CloudDownloadIcon from '@mui/icons-material/CloudDownload';
interface TimeStrings {
    days: string,
    hours: string,
    minutes: string,
    seconds: string,
}
export default function BucketView(data: { container: ContainerHubsDto }) {
    const [time, setTime] = useState<TimeStrings>()
    const [shareModal, setShareModal] = useState<boolean>(false)
    const [shareSnackbar, setShareSnackbar] = useState<boolean>(false)
    const [copyLink, setCopyLink] = useState<string>()
    const router = useRouter()

    useEffect(() => {
        CountDownTimer(data.container.expiresAt, "")
    }, [data.container.expiresAt])

    useEffect(() => {
        if (!window?.location?.href) return

        setCopyLink(window.location.href)
    }, [])

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

            setTime({
                days: days.toString().length == 1 ? "0" + days : days.toString(),
                hours: hours.toString().length == 1 ? "0" + hours : hours.toString(),
                minutes: minutes.toString().length == 1 ? "0" + minutes : minutes.toString(),
                seconds: seconds.toString().length == 1 ? "0" + seconds : seconds.toString()
            })

            //setTime(`${days.toString().length == 1 ? "0" + days : days} : ${hours.toString().length == 1 ? "0" + hours : hours} : ${minutes.toString().length == 1 ? "0" + minutes : minutes} : ${seconds.toString().length == 1 ? "0" + seconds : seconds}`)
            //setTime(`${days} days, ${hours} hours, ${minutes} minutes, ${seconds} seconds`)
        }

        timer = setInterval(showRemaining, 1000);
    }



    return (
        <div className={styles.homeContainer}>
            <div className={styles.lowerRightShape} />
            <div className={styles.upperRightShape} />
            <div className={styles.bearWithTable} />
            <Container sx={{
                backgroundColor: {
                    xs: "#F5D1B4",
                    md: "#FFEAD9"
                }
            }} maxWidth={false}>
                <Grid container >
                    <Grid xs={12} md={8}>

                        <Box sx={{
                            height: "100%",
                            justifyContent: { xs: "center", md: "flex-start" }
                        }}>
                            <Stack
                                direction="column"
                                alignItems="center"
                                spacing={2}>
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
                                    }} justifyContent="center" alignItems={"center"}>

                                        <b>You joined a bucket</b>
                                        <HiveIcon height={60} width={60} />
                                    </Box>

                                    <Box sx={{
                                        fontSize: {
                                            xs: "1.3rem",
                                            md: "1.5rem",
                                        },
                                        display: "flex",
                                        alignItems: "center",
                                    }} mt={2}>
                                        Bucket expires in:
                                        <Box ml={1} component="span">
                                            <input disabled className={styles.customInputBox} value={time?.days ?? "00"} />
                                            :
                                            <input disabled className={styles.customInputBox} value={time?.hours ?? "00"} />
                                            :
                                            <input disabled className={styles.customInputBox} value={time?.minutes ?? "00"} />
                                            :
                                            <input disabled className={styles.customInputBox} value={time?.seconds ?? "00"} />
                                            {/* {time} */}
                                        </Box>
                                    </Box>

                                </Typography>

                                {/* List here */}
                                <TableContainer sx={{
                                    backgroundColor: "#E2B590",
                                    // width: "50vw",
                                    //minWidth: "50vw",
                                    maxWidth: "80%",
                                    zIndex: 50,
                                    maxHeight: "60vh",
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
                                                <TableCell align="right"
                                                >
                                                    Download
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
                                                    <TableCell align="right">
                                                        <IconButton sx={{
                                                            backgroundColor: "#CD9A71"
                                                        }} download={e.fileName} href={e.signedItemUrl}>
                                                            <CloudDownloadIcon />
                                                        </IconButton>
                                                    </TableCell>
                                                </TableRow>
                                            )}

                                        </TableBody>
                                    </Table>
                                </TableContainer>

                                <Button size="large" onClick={() => setShareModal(true)} endIcon={<ShareIcon />} color='secondary' variant="contained">Share bucket</Button>
                            </Stack>
                        </Box>
                    </Grid>
                    <Grid xs={12} md={4}>
                        <Stack spacing={2} sx={{
                            marginTop: { xs: "25%", lg: "20%" },
                            alignItems: "center",
                        }}>
                            <Typography
                                variant="h4"
                                sx={{
                                    fontFamily: 'Aref Ruqaa Ink',
                                    maxWidth: "80%",
                                    zIndex: 1,
                                    color: "#502720",
                                    textAlign: "center",
                                }}
                            >
                                Do you want to create<br />your own bucket?
                            </Typography>

                            <Button size="large" endIcon={<HoneyPotIcon />} onClick={() => {
                                router.push("/")
                            }} color='secondary' variant="contained">Create a bucket</Button>
                        </Stack>
                    </Grid>
                </Grid>
            </Container>

            {/* Share bucket modal */}
            <Dialog
                open={shareModal}
                onClose={() => setShareModal(false)}
                aria-labelledby="alert-dialog-title"
                aria-describedby="alert-dialog-description"
            >
                <DialogTitle id="alert-dialog-title">
                    Share bucket
                </DialogTitle>
                <DialogContent>
                    <DialogContentText id="alert-dialog-description">
                        Do you want to share this bucket with another person? Simply give them the following link and they will have access!
                    </DialogContentText>
                    <Grid container spacing={2} alignItems={"center"}>
                        <Grid xs={10}>
                            <TextField
                                autoFocus
                                disabled
                                sx={{
                                    "& .MuiInputBase-input.Mui-disabled": {
                                        WebkitTextFillColor: "#000000",
                                    },
                                }}
                                margin="dense"
                                type="email"
                                fullWidth
                                variant="standard"
                                value={copyLink}
                            />
                        </Grid>
                        <Grid xs={2}>
                            <Button variant="outlined" color="secondary" onClick={() => {
                                navigator.clipboard.writeText(copyLink ?? "")
                                setShareSnackbar(true)
                            }} autoFocus>
                                COPY
                            </Button>
                        </Grid>


                        <Grid xs={12}>
                            Or you can just use this code on our homepage: {data.container.shortCodeString}
                        </Grid>
                    </Grid>
                </DialogContent>
                <DialogActions>
                    <Button variant="contained" onClick={() => setShareModal(false)} autoFocus>
                        Close
                    </Button>
                </DialogActions>
            </Dialog>

            <Snackbar
                open={shareSnackbar}
                autoHideDuration={3000}
                onClose={() => setShareSnackbar(false)}
                message="Share link copied!"
            >
                <Alert severity="success" sx={{ width: '100%' }}>
                    This is a success message!
                </Alert>
            </Snackbar>

        </div>
    )
}