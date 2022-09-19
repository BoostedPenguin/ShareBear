import { createTheme } from '@mui/material/styles';
import { red } from '@mui/material/colors';

// Create a theme instance.
const theme = createTheme({
  palette: {
    primary: {
      main: '#E2B590',
      contrastText: "#5D2F27"
    },
    text: {
        primary: "#5D2F27"
    },
    secondary: {
      main: '#502F57',
    },
    error: {
      main: red.A400,
    },
  },
});

export default theme;