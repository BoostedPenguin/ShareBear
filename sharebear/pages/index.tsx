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
const Home: NextPage = () => {
  return (
    <div className={styles.homeContainer}>
      <div className={styles.lowerRightShape} />
      <div className={styles.upperRightShape} />
      <div className={styles.bearWithTable} />
      <Container maxWidth="xl">
        <Stack
          direction="column"
          justifyContent="center"
          alignItems="center"
          spacing={4}>

          <Button size="large" endIcon={<HoneyPotIcon />} color='secondary' variant="contained">Create a bucket</Button>
          <Button size="medium" color='secondary' variant="contained">Join a bucket</Button>
        </Stack>
      </Container>
    </div>
  )
}

export default Home
