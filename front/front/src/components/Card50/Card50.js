import React from "react";
import { makeStyles, useTheme } from "@material-ui/core/styles";
import Card from "@material-ui/core/Card";
import CardContent from "@material-ui/core/CardContent";
import CardMedia from "@material-ui/core/CardMedia";
import IconButton from "@material-ui/core/IconButton";
import Typography from "@material-ui/core/Typography";
import SkipPreviousIcon from "@material-ui/icons/SkipPrevious";
import PlayArrowIcon from "@material-ui/icons/PlayArrow";
import SkipNextIcon from "@material-ui/icons/SkipNext";
import Btn from "../Buttons/Button";
import Hidden from "@material-ui/core/Hidden";

const useStyles = makeStyles((theme) => ({
  root: {
    display: "flex",
    justifyContent: "center",
    backgroundColor: "transparent",
    border: "none",
    boxShadow: "none",
    "@media (max-width: 780px)": {
      display: "block",
      margin: "0 auto",
    },
  },
  details: {
    display: "flex",
    flexDirection: "column",
    verticalAlign: "middle",
    justifyContent: "center",
    "@media (max-width: 780px)": {
      width: "100%",
      display: "block",
    },
  },
  content: {
    flex: "1 0 auto",
  },
  image: {
    width: "100%",
    height: "auto",
  },
  cardContainer: {
    width: "100%",
    margin: "2rem auto",
  },
  leftSide: {
    width: "50%",
    "@media (max-width: 780px)": {
      width: "100%",
      display: "block",
    },
  },
  rightSide: {
    width: "50%",
    padding: theme.spacing(10, 0),
    "@media (max-width: 780px)": {
      width: "100%",
      display: "block",
      padding: theme.spacing(0, 0),
    },
  },
  smallVSpace: {
    padding: theme.spacing(5, 0),
  },
}));

export default function Card50(props) {
  const classes = useStyles();
  const theme = useTheme();
  const visibility = props.visibility;
  const clas = classes.visibility;

  return (
    <div className={classes.cardContainer}>
      <Card className={classes.root}>
        <div className={classes.leftSide}>
          <CardMedia src={props.src} title="Live from space album cover" />
          <img src={props.src} className={classes.image}></img>
        </div>

        <div className={(classes.details, classes.rightSide)}>
          <CardContent className={classes.content}>
            <Typography component="h1" variant="h2">
              {props.title}
            </Typography>
            <Typography
              variant="h4"
              color="textSecondary"
              className={classes.smallVSpace}
            >
              {props.text}
            </Typography>
          </CardContent>
        </div>
      </Card>
    </div>
  );
}
