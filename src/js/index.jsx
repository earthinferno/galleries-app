import React from "react";
import ReactDOM from "react-dom";
import LikeButton from './like.jsx';
import Image from './image-view/image.jsx';

const Index = () => {
    return <div>
        <Image/>
        <LikeButton/>
    </div>;
}

ReactDOM.render(<Index/>, document.getElementById('reactIndex'));
