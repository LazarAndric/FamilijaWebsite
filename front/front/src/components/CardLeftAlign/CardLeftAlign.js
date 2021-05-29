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
import "./CardLeftAlign";

const useStyles = makeStyles((theme) => ({
  root: {
    display: "flex",
    justifyContent: "center",
    backgroundColor: "transparent",
    border: "none",
    boxShadow: "none",
    padding: "0",
  },
  details: {
    display: "flex",
    flexDirection: "column",
    verticalAlign: "middle",
    padding: "0",
  },
  content: {
    flex: "1 0 auto",
    "@media (max-width: 780px)": {
      padding: "1rem 0rem 1rem 1rem",
    },
  },
  image: {
    width: "80%",
    height: "auto",
    "@media (max-width: 780px)": {
      width: "100%",
      margin: "1.5rem auto",
    },
  },
  cardContainer: {
    width: "100%",
    margin: "2rem auto",
  },
  leftSide: {
    width: "20%",
  },
  rightSide: { width: "80%" },
}));

export default function CardLeft(props) {
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
            <Typography component="h5" variant="h5">
              {props.title}
            </Typography>
            <Typography variant="h6" color="textSecondary">
              {props.text}
            </Typography>
          </CardContent>
        </div>
      </Card>
    </div>
  );
}
