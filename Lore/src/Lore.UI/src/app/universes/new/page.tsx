'use client'

import { useRouter } from 'next/navigation'
import { ArrowLeft } from 'lucide-react'
import { UniverseForm } from '@/components/universes/UniverseForm'
import { useCreateUniverse } from '@/hooks/useUniverses'
import type { CreateUniversePayload, UpdateUniversePayload } from '@/lib/api/universes'

export default function NewUniversePage() {
  const router = useRouter()
  const createUniverse = useCreateUniverse()

  const handleSubmit = async (payload: CreateUniversePayload | UpdateUniversePayload) => {
    await createUniverse.mutateAsync(payload as CreateUniversePayload)
    router.push('/universes')
  }

  return (
    <main className="max-w-2xl mx-auto px-4 py-8">
      <button
        onClick={() => router.push('/universes')}
        className="flex items-center gap-1.5 text-sm text-gray-500 hover:text-gray-700 mb-6 transition-colors"
      >
        <ArrowLeft className="w-4 h-4" />
        Back to universes
      </button>

      <div className="bg-white rounded-2xl border border-gray-100 p-6 shadow-sm">
        <h1 className="text-lg font-bold text-gray-800 mb-6">New universe</h1>
        <UniverseForm onSubmit={handleSubmit} isPending={createUniverse.isPending} />
      </div>
    </main>
  )
}
