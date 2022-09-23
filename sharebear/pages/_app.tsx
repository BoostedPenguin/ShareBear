import '../styles/globals.css'
import type { AppProps } from 'next/app'
import Head from 'next/head'
import theme from '../src/theme'
import { ThemeProvider } from '@mui/material/styles'
import { CssBaseline } from '@mui/material'
import { FpjsProvider } from '@fingerprintjs/fingerprintjs-pro-react'
import NavigationBar from '../src/components/NavigationBar'
import { Hydrate, QueryClient, QueryClientProvider } from '@tanstack/react-query'
import { useState } from 'react'
import { ReactQueryDevtools } from '@tanstack/react-query-devtools'

function MyApp({ Component, pageProps }: AppProps<{ dehydratedState: unknown }>) {
  const [queryClient] = useState(() => new QueryClient())

  return (
    <>
      <QueryClientProvider client={queryClient}>

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
            <Hydrate state={pageProps.dehydratedState}>
              <NavigationBar />
              <Component {...pageProps} />
              <ReactQueryDevtools initialIsOpen={false} />
            </Hydrate>
          </ThemeProvider>
        </FpjsProvider>
      </QueryClientProvider>
    </>
  )
  return <Component {...pageProps} />
}

export default MyApp
