const images = (state = [], action) => {
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
    case 'FETCH_IMAGES':
      return [
        ...state,
          action.images.map(image => ({
          id: image.id,
          url: image.url,
          comment: image.comment,
          liked: image.liked,
        }))
      ]
    default: 
       return state
  }
}

export default images;