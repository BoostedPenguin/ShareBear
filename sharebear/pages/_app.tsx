import '../styles/globals.css'
import type { AppProps } from 'next/app'
import Head from 'next/head'
import theme from '../src/theme'
import { ThemeProvider } from '@mui/material/styles'
import { CssBaseline } from '@mui/material'
import { FpjsProvider } from '@fingerprintjs/fingerprintjs-pro-react'
import NavigationBar from '../src/components/NavigationBar'

function MyApp({ Component, pageProps }: AppProps) {
  return (
    <>
      <FpjsProvider loadOptions={{
        apiKey: process.env.NEXT_PUBLIC_FPJS_API_KEY ?? "",
        region: "eu"
      }}>

        <Head>
          <meta name="viewport" content="initial-scale=1, width=device-width" />
        </Head>

        <ThemeProvider theme={theme}>
          {/* CssBaseline kickstart an elegant, consistent, and simple baseline to build upon. */}
          <CssBaseline />
          <NavigationBar />
          <Component {...pageProps} />
        </ThemeProvider>
      </FpjsProvider>
    </>
  )
  return <Component {...pageProps} />
}

export default MyApp
