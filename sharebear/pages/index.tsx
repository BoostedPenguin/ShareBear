import Button from '@mui/material/Button'
import Box from '@mui/material/Box';
import Stack from '@mui/material/Stack';
import Container from '@mui/material/Container';
import { url } from 'inspector'
import type { NextPage } from 'next'
import NavigationBar from '../src/components/NavigationBar'
import styles from '../styles/Home.module.css'
import SendIcon from '@mui/icons-material/Send';
import { ReactComponentElement } from "react"
import HoneyPotIcon from '../src/components/icons/HoneyPotIcon'
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Unstable_Grid2';

const Home: NextPage = () => {
  return (
    <div className={styles.homeContainer}>
      <div className={styles.lowerRightShape} />
      <div className={styles.upperRightShape} />
      <div className={styles.bearWithTable} />
      <Container sx={{
        zIndex: 500
      }} maxWidth="xl">
        <Box sx={{
          position: "absolute",
          display: "flex",
          alignContent: "center",
          height: "100%"
        }}>
          <Stack
            direction="column"
            justifyContent="center"
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
              <b>ShareBear file transfer</b>
              <br />
              <Box mt={2}>
                
              Share files between any device without logging in!
              </Box>
            </Typography>
            <Button size="large" endIcon={<HoneyPotIcon />} color='secondary' variant="contained">Create a bucket</Button>
            <Button size="medium" color='secondary' variant="contained">Join a bucket</Button>
          </Stack>
        </Box>

      </Container>
    </div>
  )
}

export default Home
