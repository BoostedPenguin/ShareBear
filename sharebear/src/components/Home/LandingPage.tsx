import Button from '@mui/material/Button'
import Box from '@mui/material/Box';
import Stack from '@mui/material/Stack';
import Container from '@mui/material/Container';
import styles from '../../../styles/LandingPage.module.css'
import HoneyPotIcon from '../icons/HoneyPotIcon'
import Typography from '@mui/material/Typography';

const LandingPage = () => {
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
          alignItems: { xs: "flex-start", md: "center" },
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
                fontSize: {
                  xs: "1.5rem",
                  md: "2.125rem",
                },
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

export default LandingPage;