import React from 'react';
import Image from './image.jsx';
import { connect } from 'react-redux';
import { 
  addImage,
} from './../actions';
import { fetchImages } from './../actions/server-actions-stub';

class Images extends React.Component {

  render() {
    const images = this.props.images[0];
    if (images == null)
    {
      return(null);
    }

    return (
        <div>
          {images.map( image => (
            <Image key={image.id} url={image.url} comment={image.comment} like={image.liked}/>
          ))}
        </div>
    );      
  }
}

// const mapDispacthtoProps = (dispatch) => ({
//     image: dispatch(addImage("./../../src/img/index.png","Peace man", true))
//   });
const mapDispacthtoProps = (dispatch) => ({
    imageDispatch: dispatch(fetchImages())
    }); 

const mapStateToProps = (state) => ({
    images : state.images
  });


export default connect(
    mapStateToProps,
    mapDispacthtoProps
  )(Images);