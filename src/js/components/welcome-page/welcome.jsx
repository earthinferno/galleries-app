import React from 'react';
import centreImage from './../../../images/welcome-page-main.png'

export default class Welcome extends React.Component {
    render() {
      const leftColumn = 
        <div>
          <div className="custom_heading_size_h40">
            Benefits to signing up to <strong>mipictures</strong>
          </div>
          <p className="text-white">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed eleifend commodo odio, nec eleifend lorem rutrum in. Aliquam condimentum 
            ante lectus. Donec egestas porta nulla in aliquam. Maecenas et posuere massa. Vestibulum pharetra metus in arcu vehicula, eu vulputate turpis dictum. 
            Aliquam volutpat interdum tortor, ac maximus tortor viverra eu. Morbi nec cursus leo. Cras justo augue, condimentum et tellus sed, gravida fermentum magna. 
            Cras gravida, enim et facilisis sollicitudin, lorem ligula consequat sapien, sagittis laoreet nisi nunc a elit. Nulla venenatis sagittis tincidunt. 
            Vestibulum nibh nibh, molestie ac est eget, egestas cursus nisi. Vestibulum condimentum pellentesque aliquet. Nam vel libero vitae diam 
            congue facilisis in vel nisi.            
            </p>
        </div>;

      return (

        <div className="container-fluid bg">
          <div className="row">
            <div className="col-sm">
              {leftColumn}
            </div>
            <div className="col-sm-6">
            </div>
            <div className="col-sm">
              {leftColumn}
            </div>
          </div>
        </div>
      );
    }
  }