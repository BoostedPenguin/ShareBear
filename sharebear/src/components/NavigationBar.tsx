import * as React from 'react';
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import IconButton from '@mui/material/IconButton';
import Typography from '@mui/material/Typography';
import Menu from '@mui/material/Menu';
import MenuIcon from '@mui/icons-material/Menu';
import Container from '@mui/material/Container';
import Avatar from '@mui/material/Avatar';
import Button from '@mui/material/Button';
import Tooltip from '@mui/material/Tooltip';
import MenuItem from '@mui/material/MenuItem';
import AdbIcon from '@mui/icons-material/Adb';
import Ss from '../../public/shareBearLogo.svg';
import { scroller } from 'react-scroll'
import Image from 'next/image'
import { useRouter } from 'next/router';

const pages = ['Create', 'Join', 'About', 'FAQ', 'Contact'];

const NavigationBar = () => {
  const router = useRouter()
  const [anchorElNav, setAnchorElNav] = React.useState<null | HTMLElement>(null);

  const handleOpenNavMenu = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorElNav(event.currentTarget);
  };

  const handleCloseNavMenu = (to: string) => {
    setAnchorElNav(null);
    if (router.pathname == "/") {
      scroller.scrollTo(to, {
        duration: 1000,
        smooth: true,
      })

      return
    }
    router.push(`/`)
  };

  return (
    // maybe sticky?
    <AppBar elevation={0} position="static">
      <Container maxWidth="xl">
        <Toolbar disableGutters>
          {/* <img src="/shareBearLogo.svg" style={{
          }} /> */}
          <Box sx={{
            display: { xs: 'none', md: 'flex' },
          }}>
            <Image src="/shareBearLogo.svg" onClick={() => {
              if (router.pathname != "/")
                router.push("/")
            }} style={{
              cursor: "pointer"
            }} alt='Logo' height={"64"} width={"120"} />
          </Box>

          {/* <AdbIcon sx={{ display: { xs: 'none', md: 'flex' }, mr: 1 }} /> */}


          <Box sx={{ flexGrow: 1, display: { xs: 'flex', md: 'none' } }}>
            <IconButton
              size="large"
              aria-label="account of current user"
              aria-controls="menu-appbar"
              aria-haspopup="true"
              onClick={handleOpenNavMenu}
              color="inherit"
            >
              <MenuIcon />
            </IconButton>
            <Menu
              id="menu-appbar"
              anchorEl={anchorElNav}
              anchorOrigin={{
                vertical: 'bottom',
                horizontal: 'left',
              }}
              keepMounted
              transformOrigin={{
                vertical: 'top',
                horizontal: 'left',
              }}
              open={Boolean(anchorElNav)}
              onClose={handleCloseNavMenu}
              sx={{
                display: { xs: 'block', md: 'none' },
              }}
            >
              {pages.map((page) => (
                <MenuItem key={page} onClick={() => handleCloseNavMenu(page)}>
                  <Typography textAlign="center">{page}</Typography>
                </MenuItem>
              ))}
            </Menu>
          </Box>
          <Box sx={{
            mr: 2,
            display: { xs: 'flex', md: 'none' },
            flexGrow: 1,
            fontFamily: 'monospace',
            fontWeight: 700,
            letterSpacing: '.3rem',
            color: 'inherit',
            textDecoration: 'none',
          }}>
            <Image src="/shareBearLogo.svg" alt='Logo' height={"64"} width={"120"} />
          </Box>

          <Box sx={{ flexGrow: 1, justifyContent: "flex-end", display: { xs: 'none', md: 'flex' } }}>
            {pages.map((page) => (
              <Button
                key={page}
                onClick={() => handleCloseNavMenu(page)}
                sx={{ my: 2, color: '#5D2F27', display: 'block', mx: 2, fontWeight: 700 }}
              >
                {page}
              </Button>
            ))}
          </Box>

        </Toolbar>
      </Container>
    </AppBar>
  );
};
export default NavigationBar;
