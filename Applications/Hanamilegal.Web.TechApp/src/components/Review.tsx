import "styles/components/review.css";

interface Props {
  readonly avatarImageSource: string | undefined;
  readonly author: string;
  readonly text: string;
  readonly className?: string;
}

export const Review: React.FC<Props> = ({ avatarImageSource, author, text, className }: Props) => {
  return (
    <div className={`review-card ${className}`} role="article">
      <div className="review-header">
        <img className="review-avatar" src={avatarImageSource} alt="avatar" />
        <div className="review-author">{author}</div>
      </div>
      <div className="review-separator" />
      <div className="review-text">{text}</div>
    </div>
  );
};
