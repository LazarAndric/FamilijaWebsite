import React, { Component } from "react";

class Log extends Component {
  state={
    
  }
  constructor(props) {
    super(props);
    this.state = {
      loading: true,
      jwt:null,
      refresh:null
    };
  }
  async componentDidMount() {
    const payload = {
      username: "lazarndrc@gmail.com",
      password: "unL@ky97",
    };
    const requestOptions = {
      method: "POST",
      headers: {
        accept: "application/json",
        "Content-Type": "application/json",
      },
      body: JSON.stringify(payload),
    };
    await fetch(
      "http://herzflix.myqnapcloud.com:49300/Authorizations/logIn",
      requestOptions
    )
      .then(res=> (res.ok ? res : Promise.reject(res)))
      .then(res => res.json())
      .then(json => this.setState({refresh: json.createToken.refreshToken, jwt: json.createToken.jwtToken, loading: false}));
  }
  render() {
    return <div>{this.state.loading || !this.state.jwt || !this.state.refresh ? <div>loading...</div> : <div>{this.state.jwt}<br/> {this.state.refresh}</div>}</div>;
  }
}
export default Log;
