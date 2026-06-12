import Image from 'next/image'

interface CatDisplayProps {
  src: string
  alt?: string
}

export function CatDisplay({ src, alt = 'mochi peach cat' }: CatDisplayProps) {
  return (
    <div className="flex justify-center">
      <Image
        src={src}
        alt={alt}
        width={240}
        height={240}
        className="rounded-2xl"
        unoptimized
        priority
      />
    </div>
  )
}
