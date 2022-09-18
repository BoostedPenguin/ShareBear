import '../styles/globals.css'
import type { AppProps } from 'next/app'
import Head from 'next/head'
import theme from '../src/theme'
import { ThemeProvider } from '@mui/material/styles'
import { CssBaseline } from '@mui/material'

function MyApp({ Component, pageProps }: AppProps) {
  return (
    <>
      <Head>
        <meta name="viewport" content="initial-scale=1, width=device-width" />
      </Head>

      <ThemeProvider theme={theme}>
        {/* CssBaseline kickstart an elegant, consistent, and simple baseline to build upon. */}
        <CssBaseline />
        <Component {...pageProps} />
      </ThemeProvider>
    </>
  )
  return <Component {...pageProps} />
}

export default MyApp
