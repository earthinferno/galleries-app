import Images from './images.jsx';
import { connect } from 'react-redux';
import { addImage } from './../actions';

const mapDispacthtoProps = (dispatch) => ({
    image: dispatch(addImage("./../../src/img/index.png","Peace man", true))
  });

const mapStateToProps = (state) => ({
    images: state.images
  });

export default connect(
    mapStateToProps,
    mapDispacthtoProps
  )(Images);