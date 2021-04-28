import React from "react";
import {Link as LinkRouter} from 'react-router-dom';
import Toolbar from '@material-ui/core/Toolbar';
import Typography from '@material-ui/core/Typography';
import Button from '@material-ui/core/Button';
import AppBar from '@material-ui/core/AppBar';
import { makeStyles } from '@material-ui/core/styles';
import Sidebar from "../Sidebar/Sidebar";

const useStyles = makeStyles((theme) => ({
  '@global': {
    ul: {
      margin: 0,
      padding: 0,
      listStyle: 'none',
    },
  },

  appBar: {
    borderBottom: `1px solid ${theme.palette.divider}`,
  },

  toolbar: {
    flexWrap: 'wrap',
    justifyContent: 'space-between',
    background: '#010606',
    alignItems: 'center',
  },

  toolbarTitle: {
    color: '#fff',
    fontFamily: 'Monoton',
    fontSize: '32px',
  },

  link: {
    margin: theme.spacing(1, 1.5),
    color: '#fff',

  },
}));

const Nav = () => {
  const classes = useStyles();
  return (
    <AppBar position="relative">  
    <Toolbar className={classes.toolbar}>
          <Typography variant="h5" color="inherit" noWrap  className={classes.toolbarTitle}>
            HERZ  TV
          </Typography>
          <Toolbar className={classes.toolbar}>
            <LinkRouter to='/' className={classes.link}>
              <Typography variant="h6" color="TextPrimary" className={classes.link} > 
                  Home
              </Typography>
            </LinkRouter>
            <LinkRouter to='/iptv'>
              <Typography variant="h6" color="TextPrimary"  className={classes.link} > 
                  IPTV
              </Typography>
            </LinkRouter>
            <LinkRouter to='/about'>
              <Typography variant="h6" color="TextPrimary" className={classes.link} > 
                  About
              </Typography>
            </LinkRouter>
          </Toolbar>
          <Button href="#" color="primary" variant="outlined" className={classes.link}>
            Login
          </Button>
          {/* <Sidebar/> */}
        </Toolbar>
      </AppBar>  
  );
};

export default Nav;


// <div class="ui secondary pointing menu">
    //   <a class="active item" href="https://www.google.com">
    //     Home
    //   </a>
    //   <a class="item">Messages</a>
    //   <a class="item">Friends</a>
    //   <div class="right menu">
    //     <a class="ui item">Logout</a>
    //   </div>
    // </div>