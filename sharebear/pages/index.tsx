import { dehydrate, QueryClient } from '@tanstack/react-query';
import type { GetServerSideProps, NextPage } from 'next'
import { GetServiceFreeSpace } from '../lib/fileService';
import CreateBucket from '../src/components/Home/CreateBucket';
import JoinBucket from '../src/components/Home/JoinBucket';
import LandingPage from '../src/components/Home/LandingPage';
import PageGutter from '../src/components/Home/PageGutter';

const Home: NextPage = () => {
  return (
    <>
    <LandingPage />
    <PageGutter />
    <CreateBucket />
    <PageGutter />
    <JoinBucket />
    </>
  )
}

export async function getStaticProps() {
  const queryClient = new QueryClient()

  await queryClient.prefetchQuery(['availableStorage'], () => GetServiceFreeSpace())

  return {
    props: {
      dehydratedState: dehydrate(queryClient),
    },
  }
}

export default Home
