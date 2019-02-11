import React from 'react';
import centreImage from './../../../images/welcome-page-main.png'

export default class Welcome extends React.Component {

  componentDidMount() {
    $('#whitelistedMessage').modal('show');
  } 

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

      const whitelistedMessage = 

        <div id='whitelistedMessage' className="modal" tabIndex="-1" >
          <div className="modal-dialog" role="document">
            <div className="modal-content">
              <div className="modal-header">
                <h5 className="modal-title">Restricted Access</h5>
                <button type="button" className="close" data-dismiss="modal" aria-label="Close">
                  <span aria-hidden="true">&times;</span>
                </button>
              </div>
              <div className="modal-body">
                <p>Please note access is currently whitelisted. </p>
                <p>Please login with following credetials for a demo: </p>
                {/* <input type=''galleriesguest247@outlook.com Galleries247. */}
                <label >Username </label>
                <input type='text' readOnly className='form-control' name='UserName' value='galleriesguest247@outlook.com'/>
                <label >Password </label>
                <input type='text' readOnly className='form-control' name='Password' value='Galleries247'/>
              </div>
            </div>
          </div>


        </div>;

    return (

      <div className="container-fluid bg">
        <div>
          {whitelistedMessage}
        </div>
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