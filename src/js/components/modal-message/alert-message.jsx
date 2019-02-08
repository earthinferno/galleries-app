import React from 'react';

export default class AlertMessage extends React.Component {
    constructor(props)
    {
        super(props);
        this.onCloseButtonClick = this.onCloseButtonClick.bind(this);
    }



    onCloseButtonClick(event) {
        event.preventDefault();
        $('#alertMessage').modal('hide');
        this.props.onMessageClose();
    }

    componentDidMount() {
        $('#alertMessage').modal('show');
    }
   
    render(){
        const modal = 
            <div id='alertMessage' className="modal" tabIndex="-1" role="dialog">
                <div className="modal-dialog" role="document">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h5 className="modal-title">Oops! Something went wrong.</h5>
                            {/* <h5 className="alert alert-warning">Oops! Something went wrong.</h5> */}
                        </div>
                        <div className="modal-body">
                            <p>{this.props.messageBody}</p>
                        </div>
                        <div className="modal-footer">
                            <button type="button" className="btn btn-outline-primary" onClick={this.onCloseButtonClick} >Close</button>
                           
                        </div>
                    </div>
                </div>
            </div>;

        return (
             <div>
                {modal}
             </div>
        );
    }
}
