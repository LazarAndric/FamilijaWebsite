import React from 'react';
import style from './CardLeftAlign.css'
import Button from '../Buttons/Button'

const CardLeftAlign = props => {
    return (
<div class="row g-0">
    <div className="col-md-6">
      <img src="https://www.cwu.org/wp-content/uploads/2017/04/540x300-placeholder.png" alt="..." style={{ width : '100%'}}/>
    </div>
    <div className="col-md-6">
      <div  className="card-body">
        <h5 className="card-title">{props.title}</h5>
        <p className="card-text">{props.text}</p>
        <p className="card-text"><small class="text-muted">{props.smallText}</small></p>
        <Button type={props.type} text={props.buttonText}/>
      </div>
    </div>
  </div>
    );
};

export default CardLeftAlign;