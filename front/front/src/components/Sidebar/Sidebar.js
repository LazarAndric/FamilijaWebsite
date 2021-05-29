import React from "react";
import clsx from "clsx";
import { makeStyles, useTheme } from "@material-ui/core/styles";
import Drawer from "@material-ui/core/Drawer";
import CssBaseline from "@material-ui/core/CssBaseline";
import List from "@material-ui/core/List";
import Divider from "@material-ui/core/Divider";
import IconButton from "@material-ui/core/IconButton";
import MenuIcon from "@material-ui/icons/Menu";
import ChevronLeftIcon from "@material-ui/icons/ChevronLeft";
import ChevronRightIcon from "@material-ui/icons/ChevronRight";
import ListItem from "@material-ui/core/ListItem";
import { Link as LinkRouter } from "react-router-dom";
import Button from "@material-ui/core/Button";
import Link from "@material-ui/core/Link";
import { Hidden } from "@material-ui/core";

const drawerWidth = 240;

const useStyles = makeStyles((theme) => ({
  root: {
    display: "flex",
  },
  appBar: {
    transition: theme.transitions.create(["margin", "width"], {
      easing: theme.transitions.easing.sharp,
      duration: theme.transitions.duration.leavingScreen,
    }),
  },
  appBarShift: {
    width: `calc(100% - ${drawerWidth}px)`,
    transition: theme.transitions.create(["margin", "width"], {
      easing: theme.transitions.easing.easeOut,
      duration: theme.transitions.duration.enteringScreen,
    }),
    marginRight: drawerWidth,
  },
  title: {
    flexGrow: 1,
  },

  iconStyle: {
    display: "none",

    "@media(max-Width: 780px)": {
      display: "block",
      position: "absolute",
      top: 0,
      right: 0,
      transform: "translate(-100%, 0%)",
      fontSize: "1.8rem",
      cursor: "pointer",
      color: "#fff",
    },
  },

  hide: {
    display: "none",
  },

  drawer: {
    width: drawerWidth,
    flexShrink: 0,
  },

  drawerPaper: {
    width: drawerWidth,
  },

  drawerHeader: {
    display: "flex",
    background: "#161a1d",
    alignItems: "center",
    padding: theme.spacing(0, 1),
    // necessary for content to be below app bar
    ...theme.mixins.toolbar,
    justifyContent: "flex-start",
  },

  content: {
    flexGrow: 1,
    padding: theme.spacing(3),
    background: "#161a1d",
    transition: theme.transitions.create("margin", {
      easing: theme.transitions.easing.sharp,
      duration: theme.transitions.duration.leavingScreen,
    }),
    marginRight: -drawerWidth,
  },

  contentShift: {
    transition: theme.transitions.create("margin", {
      easing: theme.transitions.easing.easeOut,
      duration: theme.transitions.duration.enteringScreen,
    }),
    marginRight: 0,
  },
}));

const Sidebar = () => {
  const classes = useStyles();
  const theme = useTheme();
  const [open, setOpen] = React.useState(false);

  const handleDrawerOpen = () => {
    setOpen(true);
  };

  const handleDrawerClose = () => {
    setOpen(false);
  };

  return (
    <div className={classes.root}>
      <CssBaseline />

      <IconButton
        color="inherit"
        aria-label="open drawer"
        edge="end"
        onClick={handleDrawerOpen}
        className={clsx(open && classes.hide)}
        className={classes.iconStyle}
        style={{ padding: "10px 0px" }}
      >
        <MenuIcon style={{ fontSize: "35px" }} />
      </IconButton>
      <Hidden>
        <Drawer
          className={classes.drawer}
          variant="persistent"
          anchor="right"
          open={open}
          classes={{
            paper: classes.drawerPaper,
          }}
        >
          <div className={classes.drawerHeader}>
            <IconButton onClick={handleDrawerClose} style={{ color: "#fff" }}>
              {theme.direction === "rtl" ? (
                <ChevronLeftIcon />
              ) : (
                <ChevronRightIcon />
              )}
            </IconButton>
          </div>
          <Divider />
          <List style={{ margin: "0 auto" }}>
            {/* {['Home', 'IPTV', 'About us',].map((text, index) => (
            <ListItem button key={text} style={{margin: "0 auto"}}>
              <ListItemText primary={text}/>
            </ListItem>
          ))} */}
            <ListItem>
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
            </ListItem>
            <ListItem>
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
            </ListItem>
            <ListItem>
              <LinkRouter to="/about">
                <Link
                  variant="button"
                  color="textPrimary"
                  href="#"
                  className={classes.link}
                >
                  About
                </Link>
              </LinkRouter>
            </ListItem>
          </List>
          <Divider />
          <List>
            <ListItem>
              <LinkRouter to="/login" style={{ margin: "0 auto" }}>
                <Button
                  href="#"
                  color="primary"
                  variant="outlined"
                  className={classes.link}
                >
                  Login
                </Button>
              </LinkRouter>
            </ListItem>
          </List>
        </Drawer>
      </Hidden>
    </div>
  );
};

export default Sidebar;
