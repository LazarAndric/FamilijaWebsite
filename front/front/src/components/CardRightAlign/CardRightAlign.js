import React from 'react';

const CardRightAlign = () => {
    return (
<div class="row g-0">

    <div class="col-md-6">
      <div class="card-body">
        <h5 class="card-title">Card title</h5>
        <p class="card-text">This is a wider card with supporting text below as a natural lead-in to additional content. This content is a little bit longer.</p>
        <p class="card-text"><small class="text-muted">Last updated 3 mins ago</small></p>
      </div>
    </div>
    <div class="col-md-6">
      <img src="https://www.cwu.org/wp-content/uploads/2017/04/540x300-placeholder.png" alt="..." style={{ width : '100%'}}/>
    </div>
  </div>
    );
};

export default CardRightAlign;