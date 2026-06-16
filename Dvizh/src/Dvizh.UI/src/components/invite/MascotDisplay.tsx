import Image from 'next/image'

interface MascotDisplayProps {
  src: string
  alt?: string
}

export function MascotDisplay({ src, alt = 'mascot' }: MascotDisplayProps) {
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
