/*const initialState = {
  images: [{
    id: 0,
    url: "./../../src/img/index.png",
    comment: "Peace man",
    liked:  false
  }]
}*/

const initialState = [];
const images = (state = initialState, action) => {
  switch (action.type) {
    case 'ADD_IMAGE':
      return [
        ...state,
        {
          id: action.id,
          url: action.url,
          comment: action.comment,
          liked: action.liked,
        }
      ]
      default: 
        return state
  }
}

export default images;