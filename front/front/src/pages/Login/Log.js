import React, { Component } from "react";
import Cookies from 'universal-cookie';

class Log extends Component {
  constructor(props) {
    super(props);
    this.state = {
      loading: true
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
    const cookies = new Cookies();
    await fetch(
      "http://herzflix.myqnapcloud.com:49300/Authorizations/logIn",
      requestOptions
    )
      .then(res => res.json())
      .then((json) =>(
        cookies.set('refresh', json.createToken.refreshToken, {path : '/'}), 
        cookies.set('jwt', json.createToken.jwtToken, {path : '/'})))
      .then(this.setState({loading: false}));
  }
  render() {
    const cookie= new Cookies()
    const jwt=cookie.get('jwt')
    const refresh= cookie.get('refresh')
    return <div>{this.state.loading ? <div>loading...</div> : <div>{jwt}<br/> {refresh}</div>}</div>
  }
}
export default Log;
