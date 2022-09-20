import type { NextPage } from 'next'
import CreateBucket from '../src/components/Home/CreateBucket';
import LandingPage from '../src/components/Home/LandingPage';
import PageGutter from '../src/components/Home/PageGutter';

const Home: NextPage = () => {
  return (
    <>
    <LandingPage />
    <PageGutter />
    <CreateBucket />
    </>
  )
}

export default Home
