import React from "react";
import { Link as LinkRouter } from "react-router-dom";
import Toolbar from "@material-ui/core/Toolbar";
import Typography from "@material-ui/core/Typography";
import Button from "@material-ui/core/Button";
import AppBar from "@material-ui/core/AppBar";
import { makeStyles } from "@material-ui/core/styles";
import Link from "@material-ui/core/Link";
import Sidebar from "../Sidebar/Sidebar";
import { Hidden } from "@material-ui/core";
import Cookies from 'universal-cookie';

const useStyles = makeStyles((theme) => ({
  "@global": {
    ul: {
      margin: 0,
      padding: 0,
      listStyle: "none",
    },
  },

  appBar: {
    borderBottom: `1px solid ${theme.palette.divider}`,
  },
  positionSideBar: {
    display: "none",

    "@media(max-Width: 780px)": {
      display: "block",
      position: "absolute",
      top: 0,
      right: 0,
      transform: "translate(0%, 60%)",
      fontSize: "1.8rem",
      cursor: "pointer",
      color: "#fff",
    },
  },
  toolbar: {
    flexWrap: "wrap",
    justifyContent: "space-between",
    background: "#161a1d",
    alignItems: "center",
  },

  toolbarTitle: {
    color: "#fff",
    fontFamily: "Monoton",
    fontSize: "32px",
  },

  link: {
    margin: theme.spacing(1, 1.5),
    color: "#fff",
    fontSize: "16px",
  },
}));
const logOutFunction=()=>{
    const cookies = new Cookies();
    const requestOptions = {
      method: "POST",
      headers: {
        accept: "application/json",
        "Content-Type": "application/json",
        "Refresh" : cookies.get('refresh')
      },
    };
    fetch(
      "http://herzflix.myqnapcloud.com:49300/Authorizations/logOut",
      requestOptions
    )
    .then(res => console.log(res))
    cookies.remove('refresh')
    cookies.remove('jwt')
}
const Login = () =>
{
  const classes = useStyles();
  return(
      <div>
        <LinkRouter to="/Login">
          <Link
            variant="button"
            color="textPrimary"
            href="#"
            className={classes.link}
          >
            <Button
              color="primary"
              variant="outlined"
              className={classes.link}
            >
              Login
            </Button>
          </Link>
        </LinkRouter>
      </div>
  )
}

const LogOut = () =>
{
  const classes = useStyles();
  return(
  <div>
    <LinkRouter to="/">
      <Link
        variant="button"
        color="textPrimary"
        href="#"
        className={classes.link}
      >
        <Button
          color="primary"
          variant="outlined"
          className={classes.link}
          onClick={logOutFunction}
        >
          LogOut
        </Button>
      </Link>
    </LinkRouter>
  </div>
  )
}

const Nav = props => {
  const classes = useStyles();
  return (
    <AppBar position="relative">
      <Toolbar className={classes.toolbar}>
        <Typography
          variant="h5"
          color="inherit"
          noWrap
          className={classes.toolbarTitle}
        >
          HERZ TV
        </Typography>
        <Hidden xsDown>
          <Toolbar className={classes.toolbar}>
            <LinkRouter to="/" className={classes.link}>
              <Link
                variant="button"
                color="textPrimary"
                href="#"
                className={classes.link}
              >
                Home
              </Link>
            </LinkRouter>
            <LinkRouter to="/iptv">
              <Link
                variant="button"
                color="textPrimary"
                href="#"
                className={classes.link}
              >
                IPTV
              </Link>
            </LinkRouter>
            <LinkRouter to="/contact">
              <Link
                variant="button"
                color="textPrimary"
                href="#"
                className={classes.link}
              >
                Contact
              </Link>
            </LinkRouter>
          </Toolbar>
          {props.isLogin ? <Login/> : <LogOut/>}
          
        </Hidden>

        <Hidden smUp>
          <Sidebar classes={classes.positionSideBar} />
        </Hidden>
      </Toolbar>
    </AppBar>
  );
};

export default Nav;
