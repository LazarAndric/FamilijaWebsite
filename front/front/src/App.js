import React from "react";
import ReactDOM from "react-dom";
import logo from "./logo.svg";
import "./App.css";
import Nav from "./components/Nav/Nav";
import Slider from "./components/Slider/Slider";
import CardLeft from "./components/CardLeftAlign/CardLeftAlign";
import CardRight from "./components/CardRightAlign/CardRightAlign";
import Footer from "./components/Footer/Footer";
import Card from "./components/Card/Card";
import {
  CssBaseline,
  Grid,
  Container,
  AppBar,
  Toolbar,
  Typography,
  Button,
  spacing,
} from "@material-ui/core";
import { makeStyles } from "@material-ui/core/styles";

const useStyles = makeStyles((theme) => ({
  container: {
    padding: theme.spacing(0),
    paddingLeft: 0,
    margin: 0,
    overflow: "hidden",
  },
  spaceTopBottom: {
    paddingTop: theme.spacing(4),
    paddingBottom: theme.spacing(2),
  },
  overflowHidden: {
    overflow: "hidden",
  },
}));

const App = () => {
  const classes = useStyles();
  return (
    <>
      <CssBaseline />
      <AppBar position="relative">
        <Toolbar>
          <Typography>Photo</Typography>
        </Toolbar>
      </AppBar>
      <main>
        <div claseName={classes.container}>
          <Typography variant="h2" align="center" color="textPrimary">
            Hertz App
          </Typography>
          <Slider></Slider>
          <div className={classes.overflowHidden}>
            <Grid
              container
              justify="center"
              maxWidth="sm"
              className={classes.spaceTopBottom}
              alignItems="center"
              spacing={2}
            >
              <Grid item className={classes.paddingRight}>
                <Card></Card>
              </Grid>
              <Grid item className={classes.paddingRight}>
                <Card></Card>
              </Grid>
              <Grid item>
                <Card></Card>
              </Grid>
            </Grid>
          </div>
        </div>
      </main>
    </>
  );
};

export default App;
