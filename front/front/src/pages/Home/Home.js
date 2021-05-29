import React from "react";
//import logo from "./logo.svg";
//import "./App.css";
import Slider from "../../components/Slider/Slider";
import CardLeft from "../../components/CardLeftAlign/CardLeftAlign";
import CardRight from "../../components/CardRightAlign/CardRightAlign";
import Footer from "../../components/Footer/Footer";
import Card from "../../components/Card/Card";
import Card50 from "../../components/Card50/Card50";
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
import features from "./WhatYouGet.json";
import Btn from "../../components/Buttons/Button";

const useStyles = makeStyles((theme) => ({
  container: {
    padding: theme.spacing(0),
    paddingLeft: 0,
    margin: 0,
    overflow: "hidden",
    display: "block",
  },
  spaceTopBottom: {
    paddingTop: theme.spacing(5),
    paddingBottom: theme.spacing(5),
    "@media (max-width: 780px)": {
      paddingTop: theme.spacing(2),
      paddingBottom: theme.spacing(2),
    },
  },
  overflowHidden: {
    overflow: "hidden",
  },
  CardItemsContainer: {
    display: "flex",
    justifyContent: "space-between",
    alignContent: "center",
    aliignItems: "center",
    width: "68vw",
    margin: "0 auto",
    "@media (max-width: 780px)": {
      width: "90vw",
    },
  },
  fullWidthContainer: {
    width: "100vw",
    backgroundColor: "#e5e5e5",
    padding: theme.spacing(6, 0, 8, 0),
  },
}));

const Home = () => {
  const classes = useStyles();

  return (
    <>
      <CssBaseline />
      <main>
        <div className={classes.container} maxWidth>
          <Slider></Slider>
          <div className={classes.fullWidthContainer}>
            <Grid
              container
              direction="column"
              justify="flex-end"
              alignItems="center"
              className={classes.spaceTopBottom}
              spacing={4}
            >
              <Grid item lg={12}>
                <Typography variant="h2" align="center">
                  Herz App
                </Typography>
              </Grid>{" "}
              <Grid item lg={8} xs={11}>
                <Typography variant="h6" align="center">
                  Gledaj pretpremijere najgledanijih domaćih serija, kultne
                  domaće filmove i popularne sadržaje za mališane kad god
                  poželiš. Video klub je uključen u Standard i Premium
                  paket.Gledaj pretpremijere najgledanijih domaćih serija,
                  kultne domaće filmove i popularne sadržaje za mališane kad god
                  poželiš. Video klub je uključen u Standard i Premium paket.
                </Typography>
              </Grid>
              <Grid item lg={8} xs={11}>
                <Button variant="outlined" color="primary">
                  LEARN MORE
                </Button>
              </Grid>
            </Grid>
          </div>
          <div className={classes.fullWidthContainer}>
            <Grid
              container
              direction="row"
              justify="center"
              alignItems="center"
            >
              <Grid item lg={9} xs={11}>
                <Card50
                  src="https://nettvplus.com/wp-content/uploads/2020/11/in_magazin_big.png"
                  title="
                Gledaj program na svim kanalima 7 dana unazad"
                  text="Televizija se prilagođava tebi i tvom rasporedu - premotaj 7 dana unazad, pauziraj programe, koristi interaktivni TV vodič i kreiraj listu omiljenih kanala."
                ></Card50>
              </Grid>
            </Grid>
          </div>
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
            <div className={classes.fullWidthContainer}>
              <div className={classes.CardItemsContainer}>
                <Grid
                  container
                  justify="center"
                  className={classes.spaceTopBottom}
                  alignItems="center"
                  alignContent="center"
                >
                  {features.map((data) => {
                    return (
                      <>
                        <Grid
                          item
                          justify="center"
                          alignItems="center"
                          alignContent="center"
                          lg={6}
                          xs={12}
                        >
                          <CardLeft
                            src={data.srcImg}
                            title={data.title}
                            btnType="primary"
                            text={data.text}
                            visibility="hidden"
                            width="col-20"
                          ></CardLeft>
                        </Grid>
                      </>
                    );
                  })}
                </Grid>
              </div>
            </div>
          </div>
        </div>
      </main>
    </>
  );
};

export default Home;
