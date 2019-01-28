import React from 'react';
import { GalleryDataService } from './data-source';

export default class AddGallery extends React.Component {
    constructor(props)
    {
        super(props);
        this.state = {
            AddGalleryExpanded: false,
            NameValue: '',
            DescriptionValue: '',
        };

        this.OnCreateGalleryClicked = this.OnCreateGalleryClicked.bind(this);
        this.handleInputChange = this.handleInputChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.OnCreateGalleryClicked = this.OnCreateGalleryClicked.bind(this);

    }

    handleInputChange()
    {
        const target = event.target;
        const value = target.value;
        const name = target.name;
    
        this.setState({
          [name]: value
        });
    }
    
    handleSubmit(event) {
        const data = {
            Name: this.state.NameValue,
            CreatedDate: new Date(),
            Description: this.state.DescriptionValue,
            UserId: this.props.UserId,
        }
        GalleryDataService.addGallery(data, galleryData => 
            this.props.onAddGallery(galleryData)
        );        
        event.preventDefault();
    }
    
    OnCreateGalleryClicked()
    {
        let expanded = !this.state.AddGalleryExpanded;
        this.setState({AddGalleryExpanded: expanded});
    }

    render(){
        const expanded = this.state.AddGalleryExpanded;

        const form = <form onSubmit={this.handleSubmit}>
                        <div>Name: <input name='NameValue' type='text' value={this.state.Namevalue} onChange={this.handleInputChange} /></div>
                        <div>Description: <input name="DescriptionValue" type="text" value={this.state.Namevalue} onChange={this.handleInputChange} /></div>
                        <input type="submit" value="Submit" />
                    </form>;

        return (
            

            <div>
                <button onClick={this.OnCreateGalleryClicked}>{expanded ? 'Close' : 'Add Gallery'}</button>
               
                {expanded && form}
                <div>
                </div>
            </div>
        );
    }
}
